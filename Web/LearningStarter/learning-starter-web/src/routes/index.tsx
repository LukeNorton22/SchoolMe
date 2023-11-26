//This is where you will declare all of your routes (the ones that show up in the search bar)
export const routes = {
  root: `/`,
  
  user: `/user`,

  AssignmentListing: "/assignments/:id",
  AssignmentCreatee: "/assignmentCreatee/:id",
  AssignmentUpdate: "/assignmentUpdate/:id",

  AssignmentGradeListing: "/assignmentGrade/:id",
  AssignmentGradeUpdate: "/assignmentGradeUpdate/:id",
  AssignmentGradeCreate: "/assignmentGradeCreate/:id",

  FCSetCreate: "/flashcard-create/:id",
  FCQuestionCreate: `/fcquestion/create/:id`,
  FlashCardSetListing: "/FlashCardSets/:id",
  FlashCardSetUpdate: "/flashcardset-update/:id",
  FlashCardUpdate: "/flashcardUpdate/:id",
  FlashCardListing: "/flashcards/:id",
  FCUpdate: "/flashcard/update/:id",

  home: "/group",
  GroupUpdate: "/group/:id",
  groupCreate: `/group/create`,
  GroupHome: "/GroupHome/:id",

  GroupUserListing: "/groupUser",
  GroupUserCreate: "/groups/:groupId/add-user",


  MessageListing: "/messages/:id",
  MessageUpdate: "/messages/update/:id",
  MessagingPage: "/messaging/:id",

  TestListing: "/tests/:id",
  TestingPage: "/testing/:id",
  TestCreate: "/testCreate/:id",
  TestUpdate: "/testsupdate/:id",
  TestTaking: "/testTake/:id",

  QuestionCreate: `/question/create/:id`,
  QuestionUpdate: "/questionUpdate/:id",
};
