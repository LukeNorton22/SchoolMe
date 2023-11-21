import { Button, Container, Flex, Space, TextInput } from "@mantine/core"
import { useEffect, useState } from "react"
import { ApiResponse, FlashCardSetGetDto, FlashCardSetUpdateDto, FlashCardsGetDto, QuestionGetDto, QuestionUpdateDto } from "../../constants/types"
import api from "../../config/axios";
import { useNavigate, useParams } from "react-router-dom";
import { showNotification } from "@mantine/notifications";
import { FormErrors, useForm } from "@mantine/form";
import { routes } from "../../routes";

export const FCUpdate = () => {
    const [flashcard, setFlashCard] = useState<FlashCardsGetDto>();
    const navigate = useNavigate();
    const {id} = useParams();

   

    const mantineForm = useForm<FlashCardsGetDto>({
        initialValues: flashcard
    });

    useEffect(() => {
        fetchFlashCard();
        async function fetchFlashCard(){
            const response = await api.get<ApiResponse<FlashCardsGetDto>>(`/api/flashcards/${id}`);

            if(response.data.hasErrors) {
               showNotification({message: "Error finding flashcard", color: "red"});
               
            }

            if(response.data.data){
                setFlashCard(response.data.data);
                mantineForm.setValues(response.data.data);
                mantineForm.resetDirty();
            };
        
      
        }
    }, [id]);

    const submitFlashCard = async (values: QuestionUpdateDto) => {
        const response = await api.put<ApiResponse<QuestionGetDto>>(`/api/flashcards/${id}`, values);

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
            showNotification({message: "Flashcard successfully updated", color: "green"});
            navigate(routes.FlashCardSetListing.replace(":id", `${flashcard?.flashCardSetId}`));
        }

    };

    return (
        <Container>
          {flashcard && (
            <form onSubmit={mantineForm.onSubmit(submitFlashCard)}>
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
                    <Button type="submit">Submit</Button>
                    <Space w={10} />
                    <Button type="button" onClick={ () => navigate(routes.FlashCardListing.replace(":id", `${flashcard.flashCardSetId}`))}
                    >
                    Cancel
                    </Button>
                </Flex>
            </form>
            )}
        </Container>
    );
};

