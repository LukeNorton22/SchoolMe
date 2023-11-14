import { Button, Container, Flex, Space, TextInput } from "@mantine/core"
import { useEffect, useState } from "react"
import { ApiResponse, GroupUpdateDto, GroupGetDto, AssignmentGradeGetDto, AssignmentGradeUpdateDto } from "../../constants/types"
import api from "../../config/axios";
import { useNavigate, useParams } from "react-router-dom";
import { showNotification } from "@mantine/notifications";
import { FormErrors, useForm } from "@mantine/form";
import { routes } from "../../routes";

export const AssignmentGradeUpdate = () => {
    const [AssignmentGrades, setAssignmentGrades] = useState<AssignmentGradeGetDto>();
    const navigate = useNavigate();
    const {id} = useParams();

    const mantineForm = useForm<AssignmentGradeGetDto>({
        initialValues: AssignmentGrades
    });

    useEffect(() => {
        fetchAssignmentGrades();

        async function fetchAssignmentGrades(){
            const response = await api.get<ApiResponse<AssignmentGradeGetDto>>(`/api/assignmentGrade/${id}`);

            if(response.data.hasErrors) {
               showNotification({message: "Error finding AssignmentGrades", color: "red"});
               
            }

            if(response.data.data){
                setAssignmentGrades(response.data.data);
                mantineForm.setValues(response.data.data);
                mantineForm.resetDirty();
            };
        };
    }, [id]);

    const submitAssignmentGrades = async (values: AssignmentGradeUpdateDto) => {
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
            showNotification({message: "AssignmentGrades successfully updated", color: "green"});
            navigate(routes.GroupHome.replace(":id", `${AssignmentGrades?.assignmentId}`));
        }

    };

    return (
        <Container>
          {AssignmentGrades && (
            <form onSubmit={mantineForm.onSubmit(submitAssignmentGrades)}>
                <TextInput 
                    {...mantineForm.getInputProps("grades")} 
                    label = "Assignment Grade"
                    withAsterisk
                />
               
                <Space h = {18} />
                <Flex direction={"row"}>
                    <Button type="submit">Submit</Button>
                    <Space w={10} />
                    <Button type="button" onClick={ () => navigate(routes.GroupHome.replace(":id", `${AssignmentGrades.assignmentId}`))}

                    >
                    Cancel
                    </Button>
                </Flex>
            </form>
            )}
        </Container>
    );
};

