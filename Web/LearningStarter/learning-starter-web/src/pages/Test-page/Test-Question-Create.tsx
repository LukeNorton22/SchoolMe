import { FormErrors, useForm } from "@mantine/form";
import { ApiResponse, QuestionUpdateDto, TestsGetDto } from "../../constants/types";
import { Button, Container, Flex, Space, TextInput } from "@mantine/core";
import { routes } from "../../routes";
import { useNavigate, useParams } from "react-router-dom";
import { showNotification } from "@mantine/notifications";
import api from "../../config/axios";
import { useEffect, useState } from "react";

export const QuestionCreate = () => {
    const { id } = useParams();
    const [test, setTest] = useState<TestsGetDto | null>();

    const navigate = useNavigate();
    const mantineForm = useForm<QuestionUpdateDto>({
        initialValues: {
            question: "",
            answer: ""
        }
    });

    const submitQuestion = async (values: QuestionUpdateDto) => {
        if (!test) {
            return;
        }

        const request = {
            testId: test.id,
            ...values,
        };

        const response = await api.post<ApiResponse<QuestionUpdateDto>>("/api/TestQuestions", request);

        if (response.data.hasErrors) {
            const formErrors: FormErrors = response.data.errors.reduce(
                (prev, curr) => {
                    Object.assign(prev, { [curr.property]: curr.message });
                    return prev;
                },
                {} as FormErrors
            );
            mantineForm.setErrors(formErrors);
        }

        if (response.data.data) {
            showNotification({ message: "Question successfully updated", color: "green" });
            navigate(routes.TestingPage);
        }
    };

    useEffect(() => {
        fetchTests();

        async function fetchTests() {
            const response = await api.get<ApiResponse<TestsGetDto>>(`/api/Tests/${id}`);
            if (response.data.hasErrors) {
                // Handle errors here
            } else {
                setTest(response.data.data);
            }
        }
    }, [id]);

    return (
        <Container>
            <form onSubmit={mantineForm.onSubmit(submitQuestion)}>
                <TextInput
                    {...mantineForm.getInputProps("question")}
                    label="Question"
                    withAsterisk
                />
                <TextInput
                    {...mantineForm.getInputProps("answer")}
                    label="Answer"
                    withAsterisk
                />
                <Space h={18} />
                <Flex direction={"row"}>
                    <Button type="submit">Submit</Button>
                    <Space w={10} />
                    <Button
                        type="button"
                        onClick={() => {
                            navigate(routes.TestingPage.replace(":id", `${test?.id}`));
                        }}
                    >
                        Cancel
                    </Button>
                </Flex>
            </form>
        </Container>
    );
};
