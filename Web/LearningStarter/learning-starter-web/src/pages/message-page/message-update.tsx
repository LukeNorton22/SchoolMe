import { Button, Container, Flex, Space, TextInput } from "@mantine/core"
import { useEffect, useState } from "react"
import { ApiResponse, MessagesGetDto, MessagesUpdateDto } from "../../constants/types"
import api from "../../config/axios";
import { useNavigate, useParams } from "react-router-dom";
import { showNotification } from "@mantine/notifications";
import { FormErrors, useForm } from "@mantine/form";
import { routes } from "../../routes";

export const MessageUpdate = () => {
    const [message, setMessages] = useState<MessagesGetDto>();
    const navigate = useNavigate();
    const {id} = useParams();

    const mantineForm = useForm<MessagesUpdateDto>({
        initialValues: message
    });

    useEffect(() => {
        fetchMessage();

        async function fetchMessage(){
            const response = await api.get<ApiResponse<MessagesGetDto>>(`/api/Message/${id}`);

            if(response.data.hasErrors) {
               showNotification({message: "Error finding message", color: "red"});
               
            }

            if(response.data.data){
                setMessages(response.data.data);
                mantineForm.setValues(response.data.data);
                mantineForm.resetDirty();
            };
        };
    }, [id]);

    const submitMessage = async (values: MessagesUpdateDto) => {
        const response = await api.put<ApiResponse<MessagesGetDto>>(`/api/Message/${id}`, values);
        console.log('Submit button clicked');

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
            showNotification({message: "Message successfully updated", color: "green"});
            navigate(routes.GroupHome);
        }

    };

    return (
        <Container>
          {message && (
            <form onSubmit={mantineForm.onSubmit(submitMessage)}>
                <TextInput 
                    {...mantineForm.getInputProps("content")} 
                    label = "Message"
                    withAsterisk
                />
                <Space h = {18} />
                <Flex direction={"row"}>
                    <Button type="submit">Submit</Button>
                    <Space w={10} />
                    <Button type="button" onClick={ () => {navigate(routes.GroupHome);
                    }}
                    >
                    Cancel
                    </Button>
                </Flex>
            </form>
            )}
        </Container>
    );
};

