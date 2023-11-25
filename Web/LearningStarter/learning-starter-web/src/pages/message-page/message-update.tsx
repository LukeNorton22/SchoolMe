import { Button, Container, Flex, Space, TextInput } from "@mantine/core"
import { useEffect, useState } from "react"
import { ApiResponse, GroupUpdateDto, GroupGetDto, MessagesGetDto, MessagesUpdateDto } from "../../constants/types"
import api from "../../config/axios";
import { useNavigate, useParams } from "react-router-dom";
import { showNotification } from "@mantine/notifications";
import { FormErrors, useForm } from "@mantine/form";
import { routes } from "../../routes";

export const MessageUpdate = () => {
    const [Messages, setMessages] = useState<MessagesGetDto>();
    const navigate = useNavigate();
    const {id} = useParams();

    const mantineForm = useForm<MessagesGetDto>({
        initialValues: Messages
    });

    useEffect(() => {
        fetchMessages();

        async function fetchMessages(){
            const response = await api.get<ApiResponse<MessagesGetDto>>(`/api/Message/${id}`);

            if(response.data.hasErrors) {
               showNotification({message: "Error finding Messages", color: "red"});
               
            }

            if(response.data.data){
                setMessages(response.data.data);
                mantineForm.setValues(response.data.data);
                mantineForm.resetDirty();
            };
        };
    }, [id]);

    const submitMessages = async (values: MessagesUpdateDto) => {
        const response = await api.put<ApiResponse<MessagesGetDto>>(`/api/Message/${id}`, values);

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
            showNotification({message: "Messages successfully updated", color: "green"});
            navigate(routes.GroupHome.replace(":id", `${Messages?.groupId}`));
        }

    };

    return (
        <Container>
          {Messages && (
            <form onSubmit={mantineForm.onSubmit(submitMessages)}>
                <TextInput 
                    {...mantineForm.getInputProps("content")} 
                    label = "Message"
                    withAsterisk
                />
               
                <Space h = {18} />
                <Flex direction={"row"}>
                    <Button color = "yellow" type="submit">Submit</Button>
                    <Space w={10} />
                    <Button color = "yellow" type="button" onClick={ () => navigate(routes.GroupHome.replace(":id", `${Messages.groupId}`))}

                    >
                    Cancel
                    </Button>
                </Flex>
            </form>
            )}
        </Container>
    );
};

