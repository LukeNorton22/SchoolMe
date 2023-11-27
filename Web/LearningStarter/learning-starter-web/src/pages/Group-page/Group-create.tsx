import React from "react";
import { FormErrors, useForm } from "@mantine/form";
import { ApiResponse, GroupGetDto, GroupUpdateDto } from "../../constants/types";
import { Button, Container, Flex, Space, TextInput } from "@mantine/core";
import { routes } from "../../routes";
import { useNavigate } from "react-router-dom";
import { showNotification } from "@mantine/notifications";
import api from "../../config/axios";
import { useUser } from "../../authentication/use-auth";


export const GroupCreate = () => {
  const navigate = useNavigate();
  const user = useUser();

  const mantineForm = useForm<GroupUpdateDto>({
    initialValues: {
      groupName: "",
      description: "",
    },
  });

  const submitGroup = async (values: GroupUpdateDto) => {
    try {
      // Use the new endpoint for creating a group and adding the user
      const response = await api.post<ApiResponse<GroupGetDto>>(
        `/api/Groups/CreateAndAddUser?userId=${user.id}`, // Pass the user ID as a query parameter
        {
          ...values,
          creatorId: user.id, // Include the creator ID in the request payload
        }
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
          message: "Group successfully created and user added",
          color: "green",
        });
        navigate(routes.home);
      }
    } catch (error) {
      console.error("Error creating group:", error);
    }
  };

  return (
    <Container>
      <form onSubmit={mantineForm.onSubmit(submitGroup)}>
        <TextInput
          {...mantineForm.getInputProps("groupName")}
          label="Name"
          withAsterisk
          maxLength={15} 
          required
        />
        <TextInput
          {...mantineForm.getInputProps("description")}
          label="Description"
          maxLength={185} 
          maxRows={5}
        />
        <Space h={18} />
        <Flex direction={"row"}>
          <Button style={{backgroundColor:  `#F9E925`, color: `black`}} type="submit">Submit</Button>
          <Space w={10} />
          <Button
         style={{backgroundColor:  `#F9E925`, color: `black`}}
            type="button"
            onClick={() => {
              navigate(routes.home);
            }}
          >
            Cancel
          </Button>
        </Flex>
      </form>
    </Container>
  );
};
