import { Button, Container, Flex, Space, TextInput } from "@mantine/core"
import { useEffect, useState } from "react"
import { ApiResponse, AssignmentGradeGetDto, AssignmentGradeUpdateDto, FlashCardSetGetDto, FlashCardSetUpdateDto, QuestionGetDto, QuestionUpdateDto } from "../../constants/types"
import api from "../../config/axios";
import { useNavigate, useParams } from "react-router-dom";
import { showNotification } from "@mantine/notifications";
import { FormErrors, useForm } from "@mantine/form";
import { routes } from "../../routes";

export const AssignmentGradeUpdate = () => {
    const [assignmentGrade, setAssignmentGrade] = useState<AssignmentGradeGetDto>();
    const navigate = useNavigate();
    const {id} = useParams();

   

    const mantineForm = useForm<AssignmentGradeGetDto>({
        initialValues: assignmentGrade,
        
    });

    useEffect(() => {
        fetchAssignmentGrade();
        async function fetchAssignmentGrade(){
            const response = await api.get<ApiResponse<AssignmentGradeGetDto>>(`/api/assignmentGrade/${id}`);

            if(response.data.hasErrors) {
               showNotification({message: "Error finding assignment grade", color: "red"});
               
            }

            if(response.data.data){
                setAssignmentGrade(response.data.data);
                mantineForm.setValues(response.data.data);
                mantineForm.resetDirty();
            };
        
      
        }
    }, [id]);

    const submitAssignmentGrade = async (values: AssignmentGradeUpdateDto) => {
        const response = await api.put<ApiResponse<AssignmentGradeGetDto>>(`/api/assignmentGrade/${id}`, values);

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
            navigate(routes.AssignmentListing.replace(":id", `${assignmentGrade?.assignmentId}`));
        }

    };

    return (
        <Container>
          {assignmentGrade && (
            <form onSubmit={mantineForm.onSubmit(submitAssignmentGrade)}>
                <TextInput 
                    {...mantineForm.getInputProps("grades")} 
                    label = "Grade"
                    withAsterisk
                />
                
                
                <Space h = {18} />
                <Flex direction={"row"}>
                    <Button style={{backgroundColor:  `#F9E925`, color: `black`}} type="submit">Submit</Button>
                    <Space w={10} />
                    <Button style={{backgroundColor:  `#F9E925`, color: `black`}} type="button" onClick={ () => navigate(routes.AssignmentListing.replace(":id", `${assignmentGrade.assignmentId}`))}
                    >
                    Cancel
                    </Button>
                </Flex>
            </form>
            )}
        </Container>
    );
};

