// Flashcard.tsx

import React, { useState } from "react";
import { Card } from "@mantine/core";

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
      <div style={{ margin: "auto" }}>
        {isFlipped ? <span>{answer}</span> : <span>{question}</span>}
      </div>
    </Card>
  );
};

export default Flashcard;
