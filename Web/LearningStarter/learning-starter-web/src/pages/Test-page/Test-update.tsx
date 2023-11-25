import { Button, Container, Flex, Space, TextInput } from "@mantine/core"
import { useEffect, useState } from "react"
import { ApiResponse, GroupUpdateDto, GroupGetDto, TestsGetDto, TestUpdateDto } from "../../constants/types"
import api from "../../config/axios";
import { useNavigate, useParams } from "react-router-dom";
import { showNotification } from "@mantine/notifications";
import { FormErrors, useForm } from "@mantine/form";
import { routes } from "../../routes";

export const TestUpdate = () => {
    const [test, setTest] = useState<TestsGetDto>();
    const navigate = useNavigate();
    const {id} = useParams();

    const mantineForm = useForm<TestsGetDto>({
        initialValues: test
    });

    useEffect(() => {
        fetchTest();

        async function fetchTest(){
            const response = await api.get<ApiResponse<TestsGetDto>>(`/api/Tests/${id}`);

            if(response.data.hasErrors) {
               showNotification({message: "Error finding test", color: "red"});
               
            }

            if(response.data.data){
                setTest(response.data.data);
                mantineForm.setValues(response.data.data);
                mantineForm.resetDirty();
            };
        };
    }, [id]);

    const submitTest = async (values: TestUpdateDto) => {
        const response = await api.put<ApiResponse<TestsGetDto>>(`/api/Tests/${id}`, values);

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
            showNotification({message: "Test successfully updated", color: "green"});
            navigate(routes.GroupHome.replace(":id", `${test?.groupId}`));
        }

    };

    return (
        <Container>
          {test && (
            <form onSubmit={mantineForm.onSubmit(submitTest)}>
                <TextInput 
                    {...mantineForm.getInputProps("testName")} 
                    label = "Name"
                    withAsterisk
                />
               
                <Space h = {18} />
                <Flex direction={"row"}>
                    <Button color = "yellow" type="submit">Submit</Button>
                    <Space w={10} />
                    <Button color = "yellow" type="button" onClick={ () => navigate(routes.GroupHome.replace(":id", `${test.groupId}`))}

                    >
                    Cancel
                    </Button>
                </Flex>
            </form>
            )}
        </Container>
    );
};

