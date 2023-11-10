import React, { useEffect, useState } from "react";
import { showNotification } from "@mantine/notifications";
import { Button, Center, Container, Flex, Space, Table, Title, createStyles } from "@mantine/core";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useNavigate, useParams } from "react-router-dom";
import { ApiResponse, FlashCardSetGetDto } from "../../constants/types";
import { routes } from "../../routes";
import api from "../../config/axios";
import { faArrowLeft, faPen, faPlus, faTrash,  } from "@fortawesome/free-solid-svg-icons";



export const FlashCardSetListing = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const { classes } = useStyles();
  const [fcset, setFCSet] = useState<FlashCardSetGetDto | null>(null);

  async function fetchSets() {
    try {
      const response = await api.get<ApiResponse<FlashCardSetGetDto>>(`/api/FCSets/${id}`);
      if (response.data.hasErrors) {
        // Handle errors here
      } else {
        setFCSet(response.data.data);
      }
    } catch (error) {
      console.error("Error fetching sets:", error);
      showNotification({
        title: "Error",
        message: "Failed to fetch set details",
      });
    }
  }

  const handleFlashCardDelete = async (flashcardId: number) => {
    try {
      await api.delete(`/api/flashcards/${flashcardId}`);
      showNotification({ message: "Flashcard has entered the trash" });
      fetchSets();
    } catch (error) {
      console.error("Error deleting flashcard:", error);
      showNotification({
        title: "Error",
        message: "Failed to delete the question",
      });
    }
  };

  useEffect(() => {
    fetchSets();
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
          Add Flashcard
        </Button>
      <Center>
        <Title >{fcset?.setName}</Title>
        <Space h="lg" />
        </Center>
        {fcset && (
          <Table withBorder fontSize={15}>         
            <thead>
              <tr>
                <th></th>
                <th>Questions</th>
                <th>Answers</th>
              </tr>
            </thead>
            <tbody>
              {fcset.flashCards.map((flashCard, index) => (
                <tr key={index}>
                 
                 <FontAwesomeIcon
                    className={classes.iconButton}
                    icon={faPen}
                    onClick={() => {
                      navigate(
                        routes.FCUpdate.replace(":id", `${flashCard.id}`)
                      );
                    }}
                  />
                  <Button
                    onClick={() => handleFlashCardDelete(flashCard.id)}
                    color="red"
                    variant="outline"
                  >
                    <FontAwesomeIcon icon={faTrash} />
                  </Button>
                 
                  <td>{flashCard.question}</td>
                  
                    <td>
                      {flashCard.answer}
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
  