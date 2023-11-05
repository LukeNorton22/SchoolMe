import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { ApiResponse, TestsGetDto, QuestionGetDto } from "../../constants/types";
import api from "../../config/axios";
import { Button, Center, Container, Flex, Header, Space, Table, Title, createStyles } from "@mantine/core";
import { faArrowLeft, faPlus, faTruckMonster } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { routes } from "../../routes";

export const TestingPage = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const { classes } = useStyles();
  const [test, setTest] = useState<TestsGetDto | null>(null);

  useEffect(() => {
    fetchTests();

    async function fetchTests() {
      const response = await api.get<ApiResponse<TestsGetDto>>(`/api/Tests/${id}`);
      if (response.data.hasErrors) {
        // Handle errors here
      } else {
        setTest(response.data.data);
      }
    }
  }, [id]);

  return (
    <Container>
       <Button
          onClick={() => {
          navigate(routes.GroupHome.replace(":id", `${id}`));
            }
          }           
              style={{
              backgroundColor: 'transparent',
              border: 'none',
              cursor: 'pointer',
            }}
          >
            <FontAwesomeIcon icon={faArrowLeft} size="xl" /> 
          </Button>
            <Flex direction="row" justify={"space-between"}>
          <Button
          onClick={() => {
            navigate(routes.QuestionCreate.replace(":id", `${test?.id}`));
          }}
        >
          <FontAwesomeIcon icon={faPlus} /> <Space w={8} />
          Add Question
        </Button>
      </Flex>
    <Center>
      <Title >{test?.testName}</Title>
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
                  <FontAwesomeIcon
                    className={classes.iconButton}
                    icon={faTruckMonster}
                    onClick={() => {
                      navigate(routes.TestUpdate.replace(":id", `${test.id}`));
                    }}
                  />
                </td>
                <td>{question.question}</td>
                <td>
                  <td>
                    {question.answer}
                  </td>
                </td>
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
