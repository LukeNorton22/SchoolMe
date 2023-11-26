import { Button, Container, Flex, Space, TextInput } from "@mantine/core"
import { useEffect, useState } from "react"
import { ApiResponse, FlashCardSetGetDto, FlashCardSetUpdateDto } from "../../constants/types"
import api from "../../config/axios";
import { useNavigate, useParams } from "react-router-dom";
import { showNotification } from "@mantine/notifications";
import { FormErrors, useForm } from "@mantine/form";
import { routes } from "../../routes";

export const FlashCardSetUpdate = () => {
    const [fcset, setFCSet] = useState<FlashCardSetGetDto>();
    const navigate = useNavigate();
    const {id} = useParams();

    const mantineForm = useForm<FlashCardSetGetDto>({
        initialValues: fcset
    });

    useEffect(() => {
        fetchSets();

        async function fetchSets(){
            const response = await api.get<ApiResponse<FlashCardSetGetDto>>(`/api/FCSets/${id}`);

            if(response.data.hasErrors) {
               showNotification({message: "Error finding set", color: "red"});
               
            }

            if(response.data.data){
                setFCSet(response.data.data);
                mantineForm.setValues(response.data.data);
                mantineForm.resetDirty();
            };
        };
    }, [id]);

    const submitSet = async (values: FlashCardSetUpdateDto) => {
        const response = await api.put<ApiResponse<FlashCardSetGetDto>>(`/api/FCSets/${id}`, values);

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
            showNotification({message: "Set successfully updated", color: "green"});
            navigate(routes.GroupHome.replace(":id", `${fcset?.groupId}`));
        }

    };

    return (
        <Container>
          {fcset && (
            <form onSubmit={mantineForm.onSubmit(submitSet)}>
                <TextInput 
                    {...mantineForm.getInputProps("setName")} 
                    label = "Name"
                    maxLength={25} 
                    withAsterisk
                />
               
                <Space h = {18} />
                <Flex direction={"row"}>
                    <Button style={{backgroundColor:  `#F9E925`, color: `black`}} type="submit">Submit</Button>
                    <Space w={10} />
                    <Button style={{backgroundColor:  `#F9E925`, color: `black`}} type="button" onClick={ () => navigate(routes.GroupHome.replace(":id", `${fcset.groupId}`))}

                    >
                    Cancel
                    </Button>
                </Flex>
            </form>
            )}
        </Container>
    );
};

