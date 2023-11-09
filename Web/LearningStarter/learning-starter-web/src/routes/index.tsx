//This is where you will declare all of your routes (the ones that show up in the search bar)
export const routes = {
  root: `/`,
  home: `/home`,
  user: `/user`,

  AssignmentListing: "/assignments/:id",
  AssignmentCreatee: "/assignmentCreatee/:id",

  AssignmentGradeListingg: "/assignmentGradeListing/:id",
  AssignmentGradeUpdate: "/assignmentGrade/:id",
  AssignmentGradeCreate: "/assignmentGradeCreate/:id",

  FCSetCreate: "/flashcard-create/:id",
  FCQuestionCreate: `/fcquestion/create/:id`,
  FlashCardSetListing: "/FlashCardSets/:id",
  FlashCardSetUpdate: "/flashcardset-update/:id",
  FlashCardUpdate: "/flashcards/:id",
  FlashCardListing: "/flashcards",

  GroupListing: "/group",
  GroupUpdate: "/group/:id",
  groupCreate: `/group/create`,
  GroupHome: '/GroupHome/:id',

  MessageListing: "/message/:id",
  MessageUpdate: "/message/:id",

  TestListing: "/tests/:id",
  TestingPage: "/testing/:id",
  TestCreate: "/testCreate/:id",
  TestUpdate: "/testsupdate/:id",

  QuestionCreate: `/question/create/:id`,


  
};
