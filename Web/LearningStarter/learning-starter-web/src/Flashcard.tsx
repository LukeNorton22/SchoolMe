import React, { useState } from 'react';
import styles from './Flashcard.module.css';
const Flashcard = ({ question, answer }) => {
  const [showAnswer, setShowAnswer] = useState(false);

  const handleToggleAnswer = () => {
    setShowAnswer(!showAnswer);
  };

  return (
    <div className={`${styles.flashcard}`}> {/* Apply local styles */}
      <div className={`${styles.card} ${showAnswer ? styles.flip : ''}`} onClick={handleToggleAnswer}>
        <div className={styles['card-front']}>
         
          <p>{question}</p>
        </div>
        <div className={styles['card-back']}>
        
          <p>{answer}</p>
        </div>
      </div>
    </div>
  );
};



export default Flashcard;