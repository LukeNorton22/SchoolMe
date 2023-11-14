import { FormErrors, useForm } from "@mantine/form";
import { ApiResponse, AssignmentGradeUpdateDto, QuestionUpdateDto, TestsGetDto } from "../../constants/types";
import { Button, Container, Flex, Space, TextInput } from "@mantine/core";
import { routes } from "../../routes";
import { useNavigate, useParams } from "react-router-dom";
import { showNotification } from "@mantine/notifications";
import api from "../../config/axios";


export const GradeCreate = () => {
    const navigate = useNavigate();
    const {  id } = useParams();
    const mantineForm = useForm<AssignmentGradeUpdateDto>({
      initialValues: {
        grade: 0
      },
    });
  
    const submitGrade = async (values: AssignmentGradeUpdateDto) => {
      const response = await api.post<ApiResponse<AssignmentGradeUpdateDto>>(
        `/api/assignmentGrade/${id}`, 
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
        showNotification({ message: "New grade added", color: "purple" });
        navigate(routes.AssignmentListing.replace(":id", `${id}`));
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
