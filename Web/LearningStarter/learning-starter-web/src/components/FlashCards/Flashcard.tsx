// Flashcard.tsx

import React, { useState } from "react";
import { Card, Button } from "@mantine/core";

interface FlashcardProps {
  question: string;
  answer: string;
}

const Flashcard: React.FC<FlashcardProps> = ({ question, answer }) => {
  const [isFlipped, setFlipped] = useState(false);

  return (
    <Card
      shadow="lg"
      style={{ width: "600px", height: "400px", margin: "auto" }}
      onClick={() => setFlipped(!isFlipped)}
    >
      <div style={{ height: "100%", display: "flex", alignItems: "center", justifyContent: "center" }}>
        {isFlipped ? <span>{answer}</span> : <span>{question}</span>}
      </div>
    </Card>
  );
};

export default Flashcard;
