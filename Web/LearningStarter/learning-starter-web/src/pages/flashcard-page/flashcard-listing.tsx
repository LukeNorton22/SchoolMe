import { useEffect, useState } from "react";
import { ApiResponse, FlashCardsGetDto } from "../../constants/types";
import { showNotification } from "@mantine/notifications";
import api from "../../config/axios";
import { Container, Header, Space, Table, createStyles } from "@mantine/core";
import { faPencil } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useNavigate } from "react-router-dom";
import { routes } from "../../routes";

export {};
export const FlasCardListing = () => {
  const [flashcards, setFlashCards] = useState<FlashCardsGetDto[]>();
  const navigate = useNavigate();
  const { classes } = useStyles();
  useEffect(() => {
    fetchFlashCards();

    async function fetchFlashCards() {
      const response = await api.get<ApiResponse<FlashCardsGetDto[]>>(
        "/api/flashcards"
      );

      if (response.data.hasErrors) {
        showNotification({ message: "Error fetching flashcards." });
      }
      if (response.data.data) {
        setFlashCards(response.data.data);
      }
    }
  }, []);

  return (
    <Container>
      <Header height={32}>FlashCards</Header>
      <Space h="md" />
      {flashcards && (
        <Table withBorder striped>
          <thead>
            <tr>
              <th></th>
              <th>Question</th>
              <th>Answer</th>
            </tr>
          </thead>
          <tbody>
            {flashcards.map((flashcard) => {
              return (
                <tr>
                  <td>
                    <FontAwesomeIcon
                      className={classes.iconButton}
                      icon={faPencil}
                      onClick={() => {
                        navigate(
                          routes.FlashCardUpdate.replace(
                            ":id",
                            "{flashcard.id}"
                          )
                        );
                      }}
                    />
                  </td>
                  <td>{flashcard.question}</td>
                  <td>{flashcard.answer}</td>
                </tr>
              );
            })}
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
