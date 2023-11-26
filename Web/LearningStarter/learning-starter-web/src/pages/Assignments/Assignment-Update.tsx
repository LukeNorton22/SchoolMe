import { Button, Container, Flex, Space, TextInput } from "@mantine/core"
import { useEffect, useState } from "react"
import { ApiResponse, AssignmentGetDto, AssignmentUpdateDto } from "../../constants/types"
import api from "../../config/axios";
import { useNavigate, useParams } from "react-router-dom";
import { showNotification } from "@mantine/notifications";
import { FormErrors, useForm } from "@mantine/form";
import { routes } from "../../routes";

export const AssignmentUpdate = () => {
    const [assignment, setAssignment] = useState<AssignmentGetDto>();
    const navigate = useNavigate();
    const {id} = useParams();

    const mantineForm = useForm<AssignmentGetDto>({
        initialValues: assignment
    });

    useEffect(() => {
        fetchAssignment();

        async function fetchAssignment(){
            const response = await api.get<ApiResponse<AssignmentGetDto>>(`/api/assignments/${id}`);

            if(response.data.hasErrors) {
               showNotification({message: "Error finding assignmentddd", color: "red"});
               
            }

            if(response.data.data){
                setAssignment(response.data.data);
                mantineForm.setValues(response.data.data);
                mantineForm.resetDirty();
            };
        };
    }, [id]);

    const submitAssignment = async (values: AssignmentUpdateDto) => {
        const response = await api.put<ApiResponse<AssignmentGetDto>>(`/api/assignments/${id}`, values);

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
            showNotification({message: "Assignment successfully updated", color: "green"});
            navigate(routes.GroupHome.replace(":id", `${assignment?.groupId}`));
        }

    };

    return (
        <Container>
          {assignment && (
            <form onSubmit={mantineForm.onSubmit(submitAssignment)}>
                <TextInput 
                    {...mantineForm.getInputProps("assignmentName")} 
                    maxLength={25} 
                    label = "Name"
                    withAsterisk
                />
               
                <Space h = {18} />
                <Flex direction={"row"}>
                    <Button style={{backgroundColor:  `#F9E925`, color: `black`}} type="submit">Submit</Button>
                    <Space w={10} />
                    <Button style={{backgroundColor:  `#F9E925`, color: `black`}} type="button" onClick={ () => navigate(routes.GroupHome.replace(":id", `${assignment.groupId}`))}

                    >
                    Cancel
                    </Button>
                </Flex>
            </form>
            )}
        </Container>
    );
};

