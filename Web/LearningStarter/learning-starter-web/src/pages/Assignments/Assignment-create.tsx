import { FormErrors, useForm } from "@mantine/form";
import { ApiResponse, AssignmentUpdateDto, FlashCardSetUpdateDto, QuestionUpdateDto, TestUpdateDto, TestsGetDto } from "../../constants/types";
import { Button, Container, Flex, Space, TextInput } from "@mantine/core";
import { routes } from "../../routes";
import { useNavigate, useParams } from "react-router-dom";
import { showNotification } from "@mantine/notifications";
import api from "../../config/axios";
import { useUser } from "../../authentication/use-auth";


export const AssignmentCreatee = () => {
  const navigate = useNavigate();
  const { id , userId} = useParams();
  const user = useUser();


  const mantineForm = useForm<AssignmentUpdateDto>({
    initialValues: {
      assignmentName: "",
      userId: user.id,
    },
  });

  const submitAssignment = async (values: AssignmentUpdateDto) => {
    const response = await api.post<ApiResponse<AssignmentUpdateDto>>(
      `/api/Assignments/${id}/${user.id}`, 
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
      showNotification({ message: "New assignment added", color: "purple" });
      navigate(routes.GroupHome.replace(":id" , `${id}`));
    }
  };

  return (
    <Container>
      <form onSubmit={mantineForm.onSubmit(submitAssignment)}>
        <TextInput
          {...mantineForm.getInputProps("assignmentName")}
          label="Assignment Name"
          withAsterisk
        />
        <Space h={18} />
        <Flex direction={"row"}>
          <Button color = "yellow" type="submit">Submit</Button>
          <Space w={10} />
          <Button
          color = "yellow"
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
