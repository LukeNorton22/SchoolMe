//This is where you will declare all of your routes (the ones that show up in the search bar)
export const routes = {
  root: `/`,
  home: `/home`,
  user: `/user`,
  FlashCardListing: "/flashcards",
  FlashCardUpdate: "/flashcards/:id",
  AssignmentGradeListing: "/assignmentGrade",
  AssignmentGradeUpdate: "/assignmentGrade/:id",
  MessageListing: "/message",
  MessageUpdate: "/message/:id",
  GroupListing: "/group",
  GroupUpdate: "/group/:id",
  TestListing: "/tests:id/:groupid",
  TestUpdate: "/tests/:id",
  groupCreate: `/group/create`,
  GroupHome: '/GroupHome/:id'

  
};
