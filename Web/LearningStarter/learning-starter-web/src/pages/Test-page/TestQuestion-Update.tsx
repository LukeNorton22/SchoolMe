import { Button, Container, Flex, Space, TextInput } from "@mantine/core"
import { useEffect, useState } from "react"
import { ApiResponse, FlashCardSetGetDto, FlashCardSetUpdateDto, QuestionGetDto, QuestionUpdateDto } from "../../constants/types"
import api from "../../config/axios";
import { useNavigate, useParams } from "react-router-dom";
import { showNotification } from "@mantine/notifications";
import { FormErrors, useForm } from "@mantine/form";
import { routes } from "../../routes";

export const TestQuestionUpdate = () => {
    const [question, setQuestion] = useState<QuestionGetDto>();
    const navigate = useNavigate();
    const {id} = useParams();

   

    const mantineForm = useForm<QuestionGetDto>({
        initialValues: question
    });

    useEffect(() => {
        fetchQuestion();
        async function fetchQuestion(){
            const response = await api.get<ApiResponse<QuestionGetDto>>(`/api/TestQuestions/${id}`);

            if(response.data.hasErrors) {
               showNotification({message: "Error finding question", color: "red"});
               
            }
            console.log("API Response:", response.data.data);
            if(response.data.data){
                setQuestion(response.data.data);
                mantineForm.setValues(response.data.data);
                mantineForm.resetDirty();
            };
        
      
        }
    }, [id]);

    const submitQuestion = async (values: QuestionUpdateDto) => {
        const response = await api.put<ApiResponse<QuestionGetDto>>(`/api/TestQuestions/${id}`, values);

        if(response.data.hasErrors) {
            const formErrors: FormErrors = response.data.errors.reduce(
                (prev, curr) => {
                    Object.assign(prev, { [curr.property]: curr.message });
                    return prev;
                },
                {} as FormErrors
            );
                mantineForm.setErrors(formErrors);
           
        }

        if(response.data.data){
            showNotification({message: "Question successfully updated", color: "green"});
            navigate(routes.TestingPage.replace(":id", `${question?.testId}`));
        }

    };

    return (
        <Container>
          {question && (
            <form onSubmit={mantineForm.onSubmit(submitQuestion)}>
                <TextInput 
                    {...mantineForm.getInputProps("question")} 
                    label = "Question"
                    withAsterisk
                />
                 <TextInput 
                    {...mantineForm.getInputProps("answer")} 
                    label = "Answer"
                    withAsterisk
                />
                <Space h = {18} />
                <Flex direction={"row"}>
                    <Button color = "yellow" type="submit">Submit</Button>
                    <Space w={10} />
                    <Button color = "yellow" type="button" onClick={ () => navigate(routes.TestingPage.replace(":id", `${question.testId}`))}
                    >
                    Cancel
                    </Button>
                </Flex>
            </form>
            )}
        </Container>
    );
};

