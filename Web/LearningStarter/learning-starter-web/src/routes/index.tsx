//This is where you will declare all of your routes (the ones that show up in the search bar)
export const routes = {
  root: `/`,
  home: `/home`,
  user: `/user`,
  FlashCardListing: "/flashcards",
  FlashCardUpdate: "/flashcards/:id",
  FCSetCreate: "/flashcard-create/:id",
  FlashCardSetListing: "/FlashCardSets/:id",
  AssignmentGradeListing: "/assignmentGrade",
  AssignmentGradeUpdate: "/assignmentGrade/:id",
  MessageListing: "/message/:id",
  MessageUpdate: "/message/:id",
  GroupListing: "/group",
  GroupUpdate: "/group/:id",
  TestListing: "/tests/:id",
  TestingPage: "/testing/:id",
  TestCreate: "/testCreate/:id",
  TestUpdate: "/tests/:id",
  groupCreate: `/group/create`,
  QuestionCreate: `/question/create/:id`,
  FCQuestionCreate: `/fcquestion/create/:id`,

  GroupHome: '/GroupHome/:id'

  
};
