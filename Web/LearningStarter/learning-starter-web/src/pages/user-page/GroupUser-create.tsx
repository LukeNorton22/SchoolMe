import React, { useState, useEffect } from "react";
import { FormErrors, useForm } from "@mantine/form";
import { ApiResponse, GroupUserUpdateDto } from "../../constants/types";
import { Button, Container, Flex, Space, TextInput } from "@mantine/core";
import { routes } from "../../routes";
import { useNavigate, useParams } from "react-router-dom";
import { showNotification } from "@mantine/notifications";
import api from "../../config/axios";

export const AddUserToGroup = () => {
  const navigate = useNavigate();
  const { groupId } = useParams();
  const mantineForm = useForm<GroupUserUpdateDto>({
    initialValues: {
      userName: "",
    },
  });

  const [isUserAdded, setIsUserAdded] = useState(false);

  const submitAddUser = async (values: GroupUserUpdateDto) => {
    try {
      const response = await api.post<ApiResponse<GroupUserUpdateDto>>(
        `/api/Groups/${groupId}`,
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
      } else if (response.data.data) {
        showNotification({
          message: "User added to group",
          color: "purple",
        });
        setIsUserAdded(true);
        navigate(routes.GroupHome.replace(":id", `${groupId}`));
      }
    } catch (error) {
      console.error("Error adding user to group:", error);
    }
  };

  useEffect(() => {
    // Reset form when user is successfully added
    if (isUserAdded) {
      mantineForm.reset();
      setIsUserAdded(false);
    }
  }, [isUserAdded, mantineForm]);

  return (
    <Container>
      <form onSubmit={mantineForm.onSubmit(submitAddUser)}>
        <TextInput
          {...mantineForm.getInputProps("userName")}
          label="User Name"
          withAsterisk
        />
        <Space h={18} />
        <Flex direction={"row"}>
          <Button type="submit">Submit</Button>
          <Space w={10} />
          <Button
            type="button"
            onClick={() => {
              navigate(routes.GroupHome.replace(":id", `${groupId}`));
            }}
          >
            Cancel
          </Button>
        </Flex>
      </form>
    </Container>
  );
};
