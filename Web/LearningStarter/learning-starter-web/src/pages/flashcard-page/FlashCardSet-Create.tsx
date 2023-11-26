import { FormErrors, useForm } from "@mantine/form";
import { ApiResponse, FlashCardSetUpdateDto, QuestionUpdateDto, TestUpdateDto, TestsGetDto } from "../../constants/types";
import { Button, Container, Flex, Space, TextInput } from "@mantine/core";
import { routes } from "../../routes";
import { useNavigate, useParams } from "react-router-dom";
import { showNotification } from "@mantine/notifications";
import api from "../../config/axios";
import { useUser } from "../../authentication/use-auth";
import { useState } from "react";


export const FCSetCreate = () => {
  const navigate = useNavigate();
  const {  id, userId } = useParams();
  const user = useUser();
  


  const mantineForm = useForm<FlashCardSetUpdateDto>({
    initialValues: {
      setName: "",
      userId: user.id,
    },
  });

  
  const submitSet = async (values: FlashCardSetUpdateDto) => {
    const response = await api.post<ApiResponse<FlashCardSetUpdateDto>>(
      `/api/FCSets/${id}/${user.id}`, 
      values
    );
      console.log(user.id)
  
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
      showNotification({ message: "New set added", color: "purple" });
      navigate(routes.GroupHome.replace(":id" , `${id}`));
    }
  };

  return (
    <Container>
      <form onSubmit={mantineForm.onSubmit(submitSet)}>
        <TextInput
          {...mantineForm.getInputProps("setName")}
          maxLength={25} 
          label="Set Name"
          withAsterisk
        />
        <Space h={18} />
        <Flex direction={"row"}>
          <Button style={{backgroundColor:  `#F9E925`, color: `black`}} type="submit">Submit</Button>
          <Space w={10} />
          <Button
          style={{backgroundColor:  `#F9E925`, color: `black`}}
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
