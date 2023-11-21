import { FormErrors, useForm } from "@mantine/form";
import { ApiResponse, QuestionUpdateDto, TestsGetDto } from "../../constants/types";
import { Button, Container, Flex, Space, TextInput } from "@mantine/core";
import { routes } from "../../routes";
import { useNavigate, useParams } from "react-router-dom";
import { showNotification } from "@mantine/notifications";
import api from "../../config/axios";


export const QuestionCreate = () => {
  const navigate = useNavigate();
  const { test, id } = useParams();
  const mantineForm = useForm<QuestionUpdateDto>({
    initialValues: {
      question: "",
      answer: "",
    },
  });

  const submitQuestion = async (values: QuestionUpdateDto) => {
    const response = await api.post<ApiResponse<QuestionUpdateDto>>(
      `/api/TestQuestions/${id}`, 
      values
    );

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
      showNotification({ message: "New Question added", color: "purple" });
      navigate(routes.TestingPage.replace(":id", `${id}`));
    }
  };

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
              navigate(routes.TestingPage.replace(":id", `${id}`));
            }}
          >
            Cancel
          </Button>
        </Flex>
      </form>
    </Container>
  );
};
