import { FormErrors, useForm } from "@mantine/form";
import { ApiResponse, QuestionUpdateDto, TestUpdateDto, TestsGetDto } from "../../constants/types";
import { Button, Container, Flex, Space, TextInput } from "@mantine/core";
import { routes } from "../../routes";
import { useNavigate, useParams } from "react-router-dom";
import { showNotification } from "@mantine/notifications";
import api from "../../config/axios";


export const TestCreate = () => {
  const navigate = useNavigate();
  const { groupId, id } = useParams();
  const mantineForm = useForm<TestUpdateDto>({
    initialValues: {
      testName: "",
    },
  });

  const submitTest = async (values: TestUpdateDto) => {
    const response = await api.post<ApiResponse<TestUpdateDto>>(
      `/api/Tests/${id}`, 
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
      showNotification({ message: "New Test added", color: "purple" });
      navigate(routes.GroupHome.replace(":id" , `${id}`));
    }
  };

  return (
    <Container>
      <form onSubmit={mantineForm.onSubmit(submitTest)}>
        <TextInput
          {...mantineForm.getInputProps("testName")}
          label="Test Name"
          withAsterisk
        />
        <Space h={18} />
        <Flex direction={"row"}>
          <Button type="submit">Submit</Button>
          <Space w={10} />
          <Button
            type="button"
            onClick={() => {
              navigate(routes.GroupHome.replace(":id", `${id}`));
            }}
          >
            Cancel
          </Button>
        </Flex>
      </form>
    </Container>
  );
};
