import { Route, Routes as Switch, Navigate } from "react-router-dom";
import { LandingPage } from "../pages/landing-page/landing-page";
import { NotFoundPage } from "../pages/not-found";
import { useUser } from "../authentication/use-auth";
import { UserPage } from "../pages/user-page/user-page";
import { PageWrapper } from "../components/page-wrapper/page-wrapper";
import { routes } from ".";
import { FlashCardListing } from "../pages/flashcard-page/flashcard-listing";
import { FlashCardUpdate } from "../pages/flashcard-page/flashcard-update";
import { AssignmentGradeListing } from "../pages/assignmentgrade-page/assignmentgrade-listing";
import { AssignmentGradeUpdate } from "../pages/assignmentgrade-page/assignmentgrade-update";
import { GroupListing } from "../pages/Group-page/Group-listing";
import { TestListing } from "../pages/Test-page/Test-listing";

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
          <Route
            path={routes.FlashCardListing}
            element={<FlashCardListing />}
          />
          <Route path={routes.TestListing} element={<TestListing />} />
          <Route path = {routes.GroupListing} element = {<GroupListing/>} />
          <Route
            path={routes.AssignmentGradeListing}
            element={<AssignmentGradeListing />}
          />
          <Route path={routes.FlashCardUpdate} element={<FlashCardUpdate />} />
          <Route
            path={routes.AssignmentGradeUpdate}
            element={<AssignmentGradeUpdate />}
          />
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
