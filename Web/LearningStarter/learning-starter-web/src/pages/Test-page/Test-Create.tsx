import React from "react";
import { FormErrors, useForm } from "@mantine/form";
import { ApiResponse, TestUpdateDto } from "../../constants/types";
import { Button, Container, Flex, Space, TextInput } from "@mantine/core";
import { routes } from "../../routes";
import { useNavigate, useParams } from "react-router-dom";
import { showNotification } from "@mantine/notifications";
import api from "../../config/axios";
import { useUser } from "../../authentication/use-auth";

export const TestCreate = () => {
  const navigate = useNavigate();
  const { id, userId } = useParams();
  const user = useUser();

  const mantineForm = useForm<TestUpdateDto>({
    initialValues: {
      testName: "",
      userId: user.id,
    },
  });

  const submitTest = async (values: TestUpdateDto) => {
    try {
      const response = await api.post<ApiResponse<TestUpdateDto>>(
        `/api/Tests/${id}/${user.id}`,
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
        showNotification({
          message: "New test added",
          color: "purple",
        });
        navigate(routes.GroupHome.replace(":id", `${id}`));
      }
    } catch (error) {
      console.error("Error creating test:", error);
      showNotification({
        title: "Error",
        message: "Failed to create the test",
      });
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
