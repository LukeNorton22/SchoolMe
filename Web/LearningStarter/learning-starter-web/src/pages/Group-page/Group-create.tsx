import { FormErrors, useForm } from "@mantine/form";
import { ApiResponse, GroupGetDto, GroupUpdateDto } from "../../constants/types";
import { Button, Container, Flex, Space, TextInput } from "@mantine/core";
import { routes } from "../../routes";
import { useNavigate } from "react-router-dom";
import { showNotification } from "@mantine/notifications";
import api from "../../config/axios";

export const GroupCreate = () => {
const navigate = useNavigate();
    const mantineForm = useForm<GroupUpdateDto>({
        initialValues:{
            groupName: "",
            description: ""
        }
    })

    const submitGroup = async (values: GroupUpdateDto) => {
        const response = await api.post<ApiResponse<GroupGetDto>>("/api/Groups", values);

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
            navigate(routes.GroupListing);
        }

    };
    
    
    return (<Container>
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
                    <Button type="button" onClick={ () => {navigate(routes.GroupListing);
                    }}
                    >
                    Cancel
                    </Button>
                </Flex>
        </form>

    </Container>);
}