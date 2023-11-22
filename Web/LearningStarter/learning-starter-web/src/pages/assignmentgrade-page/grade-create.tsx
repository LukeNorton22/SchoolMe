import { FormErrors, useForm } from "@mantine/form";
import { ApiResponse, AssignmentGradeUpdateDto } from "../../constants/types";
import { Button, Container, Flex, Space, TextInput } from "@mantine/core";
import { useNavigate, useParams } from "react-router-dom";
import { showNotification } from "@mantine/notifications";
import api from "../../config/axios";
import { useUser } from "../../authentication/use-auth"; // Assuming you have a hook to get user information
import { routes } from "../../routes";

export const GradeCreate = () => {
  const navigate = useNavigate();
  const { id } = useParams();
  console.log("Assignment ID:", id);
  const user = useUser(); // Get user information using your hook
  const mantineForm = useForm<AssignmentGradeUpdateDto>({
    initialValues: {
      grades: 0,
    },
  });

  const submitGrade = async (values: AssignmentGradeUpdateDto) => {
    try {
      const userId = user.id;
      console.log("user.userName:", user.userName);

      console.log("userId:", user.id);
      // Ensure that user and user.id are defined
      if (!user || !user.id || !user.userName) {
        console.error("User information is missing.");
        return;
      }

      // Add userId and username to the values before sending the request
      const gradeDataWithUser = {
        ...values,
        userId: userId,
        userName: user.userName,
      };
console.log("values", values)
      const response = await api.post<ApiResponse<AssignmentGradeUpdateDto>>(
        `/api/assignmentGrade/${id}`,
        gradeDataWithUser
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
        showNotification({ message: "New grade added", color: "purple" });
        navigate(routes.AssignmentListing.replace(":id", `${id}`));
      }
    } catch (error) {
      console.error("Error submitting grade:", error);
      showNotification({
        title: "Error",
        message: "Failed to submit grade",
        color: "red",
      });
    }
  };

  return (
    <Container>
      <form onSubmit={mantineForm.onSubmit(submitGrade)}>
        <TextInput
          {...mantineForm.getInputProps("grades")}
          label="Grade"
          withAsterisk
        />

        <Space h={18} />
        <Flex direction={"row"}>
          <Button type="submit">Submit</Button>
          <Space w={10} />
          <Button
            type="button"
            onClick={() => {
              navigate(routes.AssignmentListing.replace(":id", `${id}`));
            }}
          >
            Cancel
          </Button>
        </Flex>
      </form>
    </Container>
  );
};
