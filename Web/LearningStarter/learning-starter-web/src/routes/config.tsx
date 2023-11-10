import { Route, Routes as Switch, Navigate } from "react-router-dom";
import { LandingPage } from "../pages/landing-page/landing-page";
import { NotFoundPage } from "../pages/not-found";
import { useUser } from "../authentication/use-auth";
import { UserPage } from "../pages/user-page/user-page";
import { PageWrapper } from "../components/page-wrapper/page-wrapper";
import { routes } from ".";
import { FlashCardListing } from "../pages/flashcard-page/flashcard-listing";
import { FlashCardSetUpdate } from "../pages/flashcard-page/flashcard-update";
import {   GradePage } from "../pages/assignmentgrade-page/assignmentgrade-listing";
import { AssignmentGradeUpdate } from "../pages/assignmentgrade-page/assignmentgrade-update";
import { MessageListing } from "../pages/message-page/message-listing";
import { MessageUpdate } from "../pages/message-page/message-update";
import { GroupListing } from "../pages/Group-page/Group-listing";
import { GroupUpdate } from "../pages/Group-page/Group-update";
import { TestUpdate } from "../pages/Test-page/Test-update";
import { GroupCreate } from "../pages/Group-page/Group-create";
import {GroupHome} from "../pages/Group-page/Group-home";
import { TestingPage } from "../pages/Test-page/Testing-page";
import { FlashCardSetListing } from "../pages/flashcard-page/FlashCardSet-listing";
import { QuestionCreate } from "../pages/Test-page/Test-Question-Create";
import { TestCreate } from "../pages/Test-page/Test-Create";
import { FCSetCreate } from "../pages/flashcard-page/FlashCardSet-Create";
import { FCQuestionCreate } from "../pages/flashcard-page/FlashCard-Create";
import { AssignmentListing } from "../pages/Assignments/Assignment-listing";
import {  AssignmentCreatee } from "../pages/Assignments/Assignment-create";
import { GradeCreate } from "../pages/assignmentgrade-page/grade-create";
import { AssignmentUpdate } from "../pages/Assignments/Assignment-Update";
import { TestQuestionUpdate } from "../pages/Test-page/TestQuestion-Update";
import { FCUpdate } from "../pages/flashcard-page/FCupdate";
import {TestQuestionsDisplay} from "../pages/Test-page/TestTaking";


//This is where you will tell React Router what to render when the path matches the route specified.
export const Routes = () => {
  //Calling the useUser() from the use-auth.tsx in order to get user information
  const user = useUser();
  return (
    <>
      {/* The page wrapper is what shows the NavBar at the top, it is around all pages inside of here. */}
      <PageWrapper user={user}>
        <Switch>
          {/* When path === / render LandingPage */}
          <Route path={routes.home} element={<LandingPage />} />
          {/* When path === /iser render UserPage */}
          <Route path={routes.user} element={<UserPage />} />

          <Route path={routes.TestUpdate} element={<TestUpdate />} /> 
          <Route path={routes.TestingPage} element={<TestingPage />} /> 
          <Route path={routes.TestCreate} element={<TestCreate />} /> 
          <Route path={routes.TestTaking} element={<TestQuestionsDisplay />} /> 

          <Route path={routes.QuestionCreate} element = {<QuestionCreate/>} />
          <Route path={routes.QuestionUpdate} element = {<TestQuestionUpdate/>} />

          <Route path={routes.FCQuestionCreate} element = {<FCQuestionCreate/>} />

          <Route path={routes.FlashCardSetListing} element = {<FlashCardSetListing/>} />
          <Route path={routes.FCSetCreate} element={< FCSetCreate/>} /> 
          <Route path={routes.FlashCardSetUpdate} element={< FlashCardSetUpdate/>} /> 
          <Route path={routes.FCUpdate} element={< FCUpdate/>} /> 


          <Route path={routes.GroupListing} element = {<GroupListing/>} />
          <Route path={routes.GroupUpdate} element = {<GroupUpdate/>} />
          <Route path={routes.GroupHome} element = {<GroupHome/>} />
          <Route path={routes.groupCreate} element = {<GroupCreate/>} />

          <Route path={routes.AssignmentListing} element={<AssignmentListing />}/>
          <Route path="/assignmentCreatee/:id" element={<AssignmentCreatee />} />
          <Route path={routes.AssignmentUpdate} element={<AssignmentUpdate />}/>


          <Route path={routes.AssignmentGradeListingg} element={<GradePage />}/>
          <Route path={routes.AssignmentGradeUpdate}  element={<AssignmentGradeUpdate />}/>
          <Route path={routes.AssignmentGradeCreate}  element={<GradeCreate />}/>


          <Route path={routes.MessageListing} element={<MessageListing />} />
          <Route path={routes.MessageUpdate} element={<MessageUpdate />} />

          <Route path={routes.FlashCardListing} element={<FlashCardListing />} />
          {/* <Route path={routes.FlashCardUpdate} element={<FlashCardUpdate />} /> */}
          {/* Going to route "localhost:5001/" will go to homepage */}
          <Route path={routes.root} element={<Navigate to={routes.home} />} />

          {/* This should always come last.  
            If the path has no match, show page not found */}
          <Route path="*" element={<NotFoundPage />} />
        </Switch>
      </PageWrapper>
    </>
  );
};
