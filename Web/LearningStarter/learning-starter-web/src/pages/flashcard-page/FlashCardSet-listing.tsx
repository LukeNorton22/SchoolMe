import React, { useEffect, useState } from "react";
import { showNotification } from "@mantine/notifications";
import { Button, Center, Container, Flex, Space, Table, Title, createStyles } from "@mantine/core";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useNavigate, useParams } from "react-router-dom";
import { ApiResponse, FlashCardSetGetDto } from "../../constants/types";
import { routes } from "../../routes";
import api from "../../config/axios";
import { faArrowLeft, faPlus, faTruckMonster } from "@fortawesome/free-solid-svg-icons";



export const FlashCardSetListing = () => {
    const { id } = useParams();
    const navigate = useNavigate();
    const [fcset, setFcset] = useState<FlashCardSetGetDto | null>(null);
  
    useEffect(() => {
      fetchSet();
  
      async function fetchSet() {
        const response = await api.get<ApiResponse<FlashCardSetGetDto>>(`/api/FCSets/${id}`);
        if (response.data.hasErrors) {
          
        } else {
          setFcset(response.data.data);
        }
      }
    }, [id]);
  
    return (
      <Container>
          <Button
          onClick={() => {
          navigate(routes.GroupHome.replace(":id", `${fcset?.groupId}`));
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
          <Button
          onClick={() => {
            navigate(routes.FCQuestionCreate.replace(":id", `${fcset?.id}`));
          }}
        >
          <FontAwesomeIcon icon={faPlus} /> <Space w={8} />
          Add Question
        </Button>
      <Center>
        <Title >{fcset?.setName}</Title>
        <Space h="lg" />
        </Center>
        {fcset && (
          <Table withBorder fontSize={15}>         
            <thead>
              <tr>
                
                <th>Questions</th>
                <th>Answers</th>
              </tr>
            </thead>
            <tbody>
              {fcset.flashCards.map((flashCard, index) => (
                <tr key={index}>
                  <td>{flashCard.question}</td>
                  <td>
                    <td>
                      {flashCard.answer}
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
  