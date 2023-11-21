import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import {
  ApiResponse,
  TestsGetDto,
  QuestionGetDto,
} from "../../constants/types";
import api from "../../config/axios";
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
import { routes } from "../../routes";
import { showNotification } from "@mantine/notifications";

export const TestingPage = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const { classes } = useStyles();
  const [test, setTest] = useState<TestsGetDto | null>(null);

  async function fetchTests() {
    try {
      const response = await api.get<ApiResponse<TestsGetDto>>(`/api/Tests/${id}`);
      if (response.data.hasErrors) {
        // Handle errors here
      } else {
        setTest(response.data.data);
      }
    } catch (error) {
      console.error("Error fetching tests:", error);
      showNotification({
        title: "Error",
        message: "Failed to fetch test details",
      });
    }
  }

  const handleQuestionDelete = async (questionId: number) => {
    try {
      await api.delete(`/api/TestQuestions/${questionId}`);
      showNotification({ message: "Question has entered the trash" });
      fetchTests();
    } catch (error) {
      console.error("Error deleting question:", error);
      showNotification({
        title: "Error",
        message: "Failed to delete the question",
      });
    }
  };

  useEffect(() => {
    fetchTests();
  }, [id]);

  return (
    <Container>
      <Flex direction="row" justify="space-between">
        <Flex>
          <Button
            onClick={() => {
              navigate(routes.GroupHome.replace(":id", `${test?.groupId}`));
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
              navigate(routes.QuestionCreate.replace(":id", `${test?.id}`));
            }}
          >
            <FontAwesomeIcon icon={faPlus} /> <Space w={8} />
            Add Question
          </Button>
        </Flex>
        <Flex>
          <Button
            onClick={() => {
              navigate(routes.TestTaking.replace(":id", `${test?.id}`));
            }} 
          >
            Take Test
          </Button>
        </Flex>
      </Flex>
  
      <Center>
        <Title>{test?.testName}</Title>
        <Space h="lg" />
      </Center>
  
      {test && (
        <Table withBorder fontSize={15}>
          <thead>
            <tr>
              <th></th>
              <th>Questions</th>
              <th>Answers</th>
            </tr>
          </thead>
          <tbody>
            {test.questions.map((question, index) => (
              <tr key={index}>
                <td>
                  {/* Edit Icon */}
                  <FontAwesomeIcon
                    className={classes.iconButton}
                    icon={faPen}
                    onClick={() => {
                      navigate(routes.QuestionUpdate.replace(":id", `${question.id}`));
                    }}
                    style={{ cursor: 'pointer', marginRight: '8px' }}
                  />

                  {/* Delete Icon */}
                  <FontAwesomeIcon
                    className={classes.iconButton}
                    icon={faTrash}
                    color="red"
                    size="sm"
                    onClick={() => handleQuestionDelete(question.id)}
                    style={{ cursor: 'pointer' }}
                  />
                </td>
                <td>{question.question}</td>
                <td>{question.answer}</td>
              </tr>
            ))}
          </tbody>
        </Table>
      )}
    </Container>
  );
  
};

const useStyles = createStyles(() => {
  return {
    iconButton: {
      cursor: "pointer",
    },
  };
});
