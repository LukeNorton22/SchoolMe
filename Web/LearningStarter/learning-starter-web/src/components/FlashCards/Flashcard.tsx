// Flashcard.tsx

import React, { useState } from "react";
import { Card } from "@mantine/core";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faRedo } from "@fortawesome/free-solid-svg-icons";

interface FlashcardProps {
  question: string;
  answer: string;
  isFlipped: boolean;
  onClick: () => void;
}

const Flashcard: React.FC<FlashcardProps> = ({ question, answer, isFlipped, onClick }) => {
  return (
    <Card
      shadow="lg"
      style={{ width: "600px", height: "400px", display: "flex", alignItems: "center", justifyContent: "center" }}
      onClick={onClick}
    >
      <div style={{ margin: "auto", fontSize: "25px"}}>
        {isFlipped ? <span>{answer}</span> : <span>{question}</span>}
      </div>
      <FontAwesomeIcon icon={faRedo} onClick={onClick} className="flip-icon" style={{
          position: "absolute",
          bottom: "10px",
          right: "10px",
          cursor: "pointer",
          fontSize: "23px" }}/>
    </Card>
  );
};

export default Flashcard;
