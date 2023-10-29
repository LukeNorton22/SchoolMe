import { Button, Container, Flex, Space, TextInput } from "@mantine/core"
import { useEffect, useState } from "react"
import { ApiResponse, GroupCreateUpdateDto, GroupGetDto } from "../../constants/types"
import api from "../../config/axios";
import { useNavigate, useParams } from "react-router-dom";
import { showNotification } from "@mantine/notifications";
import { useForm } from "@mantine/form";
import { routes } from "../../routes";

export const GroupUpdate = () => {
    const [group, setGroup] = useState<GroupGetDto>();
    const navigate = useNavigate();
    const {id} = useParams();

    const mantineForm = useForm<GroupCreateUpdateDto>({
        initialValues: group
    });

    useEffect(() => {
        fetchGroup();

        async function fetchGroup(){
            const response = await api.get<ApiResponse<GroupGetDto>>(`/api/Groups/${id}`);
            
            if(response.data.hasErrors){
                showNotification({message: "Error finding groups", color: "red"});
            }

            if(response.data.data){
                setGroup(response.data.data);
                mantineForm.setValues(response.data.data);
                mantineForm.resetDirty();
            };
        };
    });

    const submitGroup = async (values: GroupCreateUpdateDto) => {
        const response = await api.put<ApiResponse<GroupGetDto>>(`/api/Groups/${id}`, values);

        if(response.data.hasErrors){
            showNotification({message: "Error updating groups", color: "red"});
        }

        if(response.data.data){
            navigate(routes.GroupListing);
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
                    <Button type="button" onSubmit={ () => {navigate(routes.GroupListing);
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

