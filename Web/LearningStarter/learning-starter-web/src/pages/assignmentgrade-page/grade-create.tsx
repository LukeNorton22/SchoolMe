import React from "react";
import { FormErrors, useForm } from "@mantine/form";
import { ApiResponse, AssignmentGradeUpdateDto } from "../../constants/types";
import { Button, Container, Flex, Space, TextInput } from "@mantine/core";
import { useNavigate, useParams } from "react-router-dom";
import { showNotification } from "@mantine/notifications";
import api from "../../config/axios";
import { useUser } from "../../authentication/use-auth"; 
import { routes } from "../../routes";

export const GradeCreate = () => {
  const navigate = useNavigate();
  const { id } = useParams();
  console.log("Assignment ID:", id);
  const user = useUser(); 
  
  const mantineForm = useForm<AssignmentGradeUpdateDto>({
    initialValues: {
      grades: 0,
    },
  });

  const submitGrade = async (values: AssignmentGradeUpdateDto) => {
    try {
      // Ensure that user and user.id are defined
      if (!user || !user.id || !user.userName) {
        console.error("User information is missing.");
        return;
      }

      const gradeDataWithUser = {
        ...values,
        userId: user.id,
        userName: user.userName,
      };

      const response = await api.post<ApiResponse<AssignmentGradeUpdateDto>>(
        `/api/assignmentGrade/${id}/${user.id}`, // Include userId in the URL
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
        {/* Grade input with number type and min/max attributes */}
        <Flex align="center">
          <TextInput
            {...mantineForm.getInputProps("grades")}
            label="Grade"
            type="number"
            min={0}
            max={100}
          />
        </Flex>

        <Space h={18} />

        {/* Form submission and cancellation buttons */}
        <Flex direction={"row"}>
          <Button color="yellow" type="submit">
            Submit
          </Button>
          <Space w={10} />
          <Button
            color="yellow"
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
