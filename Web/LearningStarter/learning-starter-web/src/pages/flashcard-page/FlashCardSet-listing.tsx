import React, { useEffect, useState } from "react";
import { Button, Container, createStyles } from "@mantine/core";
import { useNavigate, useParams } from "react-router-dom";
import { FlashCardSetGetDto, ApiResponse } from "../../constants/types";
import api from "../../config/axios";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faArrowLeft, faPlus } from "@fortawesome/free-solid-svg-icons";
import Flashcard from "../../components/FlashCards/Flashcard";
import { routes } from "../../routes";

export const FlashCardListing: React.FC = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const { classes } = useStyles();
  const [fcset, setFCSet] = useState<FlashCardSetGetDto | null>(null);
  const [currentCardIndex, setCurrentCardIndex] = useState(0);

  async function fetchSet() {
    try {
      const response = await api.get<ApiResponse<FlashCardSetGetDto>>(
        `/api/FCSets/${id}`
      );
      if (!response.data.hasErrors) {
        setFCSet(response.data.data);
      }
    } catch (error) {
      console.error("Error fetching set:", error);
      // Handle errors
    }
  }

  const handleFlashCardDelete = async (flashCardId: number) => {
    try {
      const response = await api.delete<ApiResponse<any>>(`/api/flashcards/${flashCardId}`);
  
      if (response.data.hasErrors) {
        console.error("Error deleting flashcard:", response.data.errors);
        // Handle errors (maybe show a notification)
      } else {
        // Handle success (maybe show a notification)
        fetchSet();
      }
    } catch (error) {
      console.error("Error deleting flashcard:", error);
      // Handle errors (maybe show a notification)
    }
  };

  useEffect(() => {
    fetchSet();
  }, [id]);

  return (
    <Container>
      {fcset && (
        <>
          <Button
            onClick={() => navigate(`/previous-page`)}
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
              navigate(routes.FCQuestionCreate.replace(":id", `${fcset.id}`));
            }}
          >
            <FontAwesomeIcon icon={faPlus} /> Add Flashcard
          </Button>
          {fcset.flashCards.length > 0 && (
            <div style={{ textAlign: "center" }}>
              <Flashcard
                question={fcset.flashCards[currentCardIndex].question}
                answer={fcset.flashCards[currentCardIndex].answer}
              />
              <span style={{ margin: "0 16px", fontSize: "18px" }}>
                Card {currentCardIndex + 1} of {fcset.flashCards.length}
              </span>
              <Button
                onClick={() => {
                  setCurrentCardIndex((prevIndex) =>
                    prevIndex > 0 ? prevIndex - 1 : prevIndex
                  );
                }}
                className={classes.iconButton}
              >
                {"<"}
              </Button>
              <Button
                onClick={() => {
                  setCurrentCardIndex((prevIndex) =>
                    prevIndex < fcset.flashCards.length - 1
                      ? prevIndex + 1
                      : prevIndex
                  );
                }}
                className={classes.iconButton}
              >
                {">"}
              </Button>
            </div>
          )}
          {fcset.flashCards.length > 0 && (
            <table>
              <thead>
                <tr>
                  <th>Question</th>
                  <th>Answer</th>
                  <th>Actions</th>
                </tr>
              </thead>
              <tbody>
                {fcset.flashCards.map((flashCard, index) => (
                  <tr key={index}>
                    <td>{flashCard.question}</td>
                    <td>{flashCard.answer}</td>
                    <td>
                      <Button
                        onClick={() => {
                          navigate(
                            routes.FlashCardUpdate.replace(":id", `${fcset.id}`)
                          );
                        }}
                      >
                        Edit
                      </Button>
                      <Button
                        onClick={() => handleFlashCardDelete(flashCard.id)}
                      >
                        Delete
                      </Button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}
        </>
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
