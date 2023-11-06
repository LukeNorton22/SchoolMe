//This type uses a generic (<T>).  For more information on generics see: https://www.typescriptlang.org/docs/handbook/2/generics.html

import { NumberLiteralType } from "typescript";

//You probably wont need this for the scope of this class :)
export type ApiResponse<T> = {
  data: T;
  errors: ApiError[];
  hasErrors: boolean;
};

export type ApiError = {
  property: string;
  message: string;
};

export type AnyObject = {
  [index: string]: any;
};

export type UserDto = {
  id: number;
  firstName: string;
  lastName: string;
  userName: string;
};

export type FlashCardsGetDto = {
  id: number;
  flashCardSetId: number;
  question: string;
  answer: string;
};

export type AssignmentGradeGetDto = {
  id: number;
  assignmentId: number;
  creatorId: number;
  grade: number;
};

export type MessagesGetDto = {
  groupId: string;
  id: number;
  content: string;
  createdAt: string;
};

export type GroupGetDto = {
  id: number;
  groupName: string;
  description: string;
  messages: MessagesGetDto[];
  tests: TestsGetDto[];
  flashCardSets: FlashCardSetGetDto[];
  

};

export type MessagesCreateDto = {
  content: string;
};

export type MessagesUpdateDto = {
  content: string;
  createdAt: string;
};

export type GroupUpdateDto = {
  groupName: string;
  description: string;

};

export type TestsGetDto = {
  id: number;
  groupId: number;
  testName: string;
  questions: QuestionGetDto[];
  
};

export type TestUpdateDto = {
  testName: string;
};

export type FlashCardSetGetDto = {
  id: number;
  groupId: number;
  setName: string;
  flashCards: FlashCardsGetDto[];
};

export type FlashCardSetUpdateDto = {
  setName: string;

};

export type QuestionUpdateDto = {
    question: string;
    answer: string;
};

export type QuestionGetDto = {
  id: number;
  testId: number;
  question: string;
  answer: string;
};
