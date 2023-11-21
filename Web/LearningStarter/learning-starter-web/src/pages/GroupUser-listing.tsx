import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";

import {
  Button,
  Center,
  Container,
  Flex,
  Header,
  Space,
  Table,
  Title,
  createStyles,
} from "@mantine/core";
import {
  faArrowLeft,
  faPen,
  faPlus,
  faTrash,
  faTruckMonster,
} from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

import { showNotification } from "@mantine/notifications";
import { ApiResponse, GroupGetDto } from "../constants/types";
import { routes } from "../routes";
import api from "../config/axios";

export const GroupUserPage = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const { classes } = useStyles();
  const [group, setGroup] = useState<GroupGetDto | null>(null);

  async function fetchGroup() {
    try {
      const response = await api.get<ApiResponse<GroupGetDto>>(`/api/Groups/${id}`);
      if (response.data.hasErrors) {
        // Handle errors here
      } else {
        setGroup(response.data.data);
      }
    } catch (error) {
      console.error("Error fetching group:", error);
      showNotification({
        title: "Error",
        message: "Failed to fetch group details",
      });
    }
  }

  const handleQuestionDelete = async (questionId: number) => {
    try {
      await api.delete(`/api/TestQuestions/${questionId}`);
      showNotification({ message: "Question has entered the trash" });
      fetchGroup();
    } catch (error) {
      console.error("Error deleting question:", error);
      showNotification({
        title: "Error",
        message: "Failed to delete the question",
      });
    }
  };

  useEffect(() => {
    fetchGroup();
  }, [id]);

  return (
    <Container>
      <Flex direction="row" justify="space-between">
        <Flex>
          <Button
            onClick={() => {
              navigate(routes.GroupHome.replace(":id", `${group?.id}`));
            }}
            style={{
              backgroundColor: "transparent",
              border: "none",
              cursor: "pointer",
            }}
          >
            <FontAwesomeIcon icon={faArrowLeft} size="xl" />
          </Button>
          <Button
            onClick={() => {
              navigate(routes.QuestionCreate.replace(":id", `${group?.id}`));
            }}
          >
            <FontAwesomeIcon icon={faPlus} /> <Space w={8} />
            Add Question
          </Button>
        </Flex>
        <Flex>
          <Button
            onClick={() => {
              navigate(routes.TestTaking.replace(":id", `${group?.id}`));
            }} 
          >
            Take Test
          </Button>
        </Flex>
      </Flex>
  
     
  
      {group?.users.map((user) => (
        <Table withBorder fontSize={15}>
          <thead>
            <tr>
              <th></th>
              <th>Usernames</th>
             
            </tr>
          </thead>
          <tbody>
            {group.users.map((user, index) => (
              <tr key={index}>
                <td>
                  {/* Edit Icon */}
                  <FontAwesomeIcon
                    className={classes.iconButton}
                    icon={faPen}
                    onClick={() => {
                      navigate(routes.QuestionUpdate.replace(":id", `${user.id}`));
                    }}
                    style={{ cursor: 'pointer', marginRight: '8px' }}
                  />

                 
                </td>
                <td>{user.userName}</td>
               
              </tr>
            ))}
          </tbody>
        </Table>
      ))}
    </Container>
  )};
  

const useStyles = createStyles(() => {
  return {
    iconButton: {
      cursor: "pointer",
    },
  };
});
