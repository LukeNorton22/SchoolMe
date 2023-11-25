import { Button, Container, Flex, Space, TextInput } from "@mantine/core"
import { useEffect, useState } from "react"
import { ApiResponse, GroupUpdateDto, GroupGetDto } from "../../constants/types"
import api from "../../config/axios";
import { useNavigate, useParams } from "react-router-dom";
import { showNotification } from "@mantine/notifications";
import { FormErrors, useForm } from "@mantine/form";
import { routes } from "../../routes";

export const GroupUpdate = () => {
    const [group, setGroup] = useState<GroupGetDto>();
    const navigate = useNavigate();
    const {id} = useParams();

    const mantineForm = useForm<GroupUpdateDto>({
        initialValues: group
    });

    useEffect(() => {
        fetchGroup();

        async function fetchGroup(){
            const response = await api.get<ApiResponse<GroupGetDto>>(`/api/Groups/${id}`);

            if(response.data.hasErrors) {
               showNotification({message: "Error finding group", color: "red"});
               
            }

            if(response.data.data){
                setGroup(response.data.data);
                mantineForm.setValues(response.data.data);
                mantineForm.resetDirty();
            };
        };
    }, [id]);

    const submitGroup = async (values: GroupUpdateDto) => {
        const response = await api.put<ApiResponse<GroupGetDto>>(`/api/Groups/${id}`, values);

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
            showNotification({message: "Group successfully updated", color: "green"});
            navigate(routes.home);
        }

    };

    return (
        <Container>
          {group && (
            <form onSubmit={mantineForm.onSubmit(submitGroup)}>
                <TextInput 
                    {...mantineForm.getInputProps("groupName")} 
                    label = "Name"
                    withAsterisk
                />
                <TextInput 
                    {...mantineForm.getInputProps("description")} 
                    label = "Description"
                    withAsterisk
                />
                <Space h = {18} />
                <Flex direction={"row"}>
                    <Button type="submit">Submit</Button>
                    <Space w={10} />
                    <Button type="button" onClick={ () => {navigate(routes.home);
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

