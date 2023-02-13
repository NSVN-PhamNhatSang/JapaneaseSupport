import { LearnComponent } from './routes/learn/learn.component';
import { PersonalInformationComponent } from './routes/main-screen/personal-information/personal-information.component';
import { CommunityComponent } from './routes/main-screen/community/community.component';
import { AnalysisComponent } from './routes/main-screen/analysis/analysis.component';
import { MyWordbooksComponent } from './routes/main-screen/my-wordbooks/my-wordbooks.component';
import { Routes, RouterModule } from '@angular/router';
import { DashboardComponent } from './routes/main-screen/dashboard/dashboard.component';
import { MainScreenComponent } from './routes/main-screen/main-screen.component';
import { NgModule } from '@angular/core';
import { InforCourseComponent } from './routes/main-screen/personal-information/infor-course/infor-course/infor-course.component';
import { WordListComponent } from './routes/learn/word-list/word-list.component';
import { RegisterComponent } from './routes/Authorize/register/register.component';
import { CreatedWorkbookComponent } from './routes/main-screen/created-workbook/created-workbook.component';
import { InforDetailComponent } from './routes/main-screen/personal-information/infor-detail/infor-detail/infor-detail.component';
import { WordDetailComponent } from './routes/learn/word-detail/word-detail.component';
import { LoginComponent } from './routes/Authorize/login/login.component';
import { AuthGuard } from './guards/auth.guard';
import { AdminComponent } from './routes/Authorize/admin/admin.component';
import { AdminUserComponent } from './routes/Authorize/admin/admin-user/admin-user.component';
import { AdminPostComponent } from './routes/Authorize/admin/admin-post/admin-post.component';
import { AdminCourseComponent } from './routes/Authorize/admin/admin-course/admin-course.component';
import { AdminWordComponent } from './routes/Authorize/admin/admin-word/admin-word.component';
import { AdminGuard } from './guards/admin.guard';

const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: '/login' },
  {
    path: 'home',
    component: MainScreenComponent,
    children: [
      { path: 'dashboard', component: DashboardComponent,canActivate:[AuthGuard]},
      { path: 'my-wordbooks', component: MyWordbooksComponent },
      { path: 'analysis', component: AnalysisComponent },
      { path: 'community', component: CommunityComponent },
      { path: 'created-wordbooks', component: CreatedWorkbookComponent },
      { path: 'personal-information', component: PersonalInformationComponent },
      { path: 'info-detail', component: InforDetailComponent },
      { path: 'infor-course', component: InforCourseComponent },
    ],
  },
  {
    path: 'learn',
    component: LearnComponent,
    children: [
      { path: 'word-list', component: WordListComponent },
      { path: 'word-detail', component: WordDetailComponent },
    ],
  },
  {
    path: 'login',
    component: LoginComponent,
  },
  {
    path: 'register',
    component: RegisterComponent,
  },{
    path: 'admin',
    component: AdminComponent,
    canActivate:[AdminGuard],
    children : [
      {path: 'admin-user',component: AdminUserComponent},
      {path : 'admin-post',component: AdminPostComponent},
      {path: 'admin-course', component: AdminCourseComponent},
      {path:'admin-word',component: AdminWordComponent}
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
