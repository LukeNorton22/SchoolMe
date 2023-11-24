//This type uses a generic (<T>).  For more information on generics see: https://www.typescriptlang.org/docs/handbook/2/generics.html

import { NonIndexRouteObject } from "react-router-dom";
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
  assignmentId: number;
  id: number;
  grades: number;
  userId: number;
  userName: string;

};

export type MessagesGetDto = {
  groupId: number;
  userId: number;
  id: number;
  content: string;
  createdAt: string;
  userName: string;
};

export type UserGetDto = {
  userName: string;
  groups: string[];
};



export type AssignmentGetDto = {
  id: number;
  groupId: number;
  assignmentName: string;
  userId: number;
  grades: AssignmentGradeGetDto[];

};

export type AssignmentUpdateDto = {
  assignmentName: string;
  userId: number;
};

export type AssignmentGradeUpdateDto = {
  
  grades: number;

}
export type GroupGetDto = {
 
  id: number;
  groupName: string;
  description: string;
  users: GroupUserGetDto[];
  messages: MessagesGetDto[];
  tests: TestsGetDto[];
  flashCardSets: FlashCardSetGetDto[];
  assignments: AssignmentGetDto[];
  
};

export type GroupUserGetDto ={
  id: number;
  firstName: string;
  lastName: string;
  userName: string;
}

export type GroupUserUpdateDto ={
  userName: string;
}
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
  userId: number;
  questions: QuestionGetDto[];
  
};

export type TestUpdateDto = {
  testName: string;
  userId: number;
};

export type FlashCardSetGetDto = {
  id: number;
  groupId: number;
  setName: string;
  userId: number;
  flashCards: FlashCardsGetDto[];
};
export type FlashCardSetUpdateDto = {
  
  setName: string;
  userId: number;
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
