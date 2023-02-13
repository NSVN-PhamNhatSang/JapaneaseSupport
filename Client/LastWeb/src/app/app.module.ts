import { NgZorroAntdModule } from './ng-zorro-antd.module';

import { LearnComponent } from './routes/learn/learn.component';
import { PersonalInformationComponent } from './routes/main-screen/personal-information/personal-information.component';
import { CommunityComponent } from './routes/main-screen/community/community.component';
import { AnalysisComponent } from './routes/main-screen/analysis/analysis.component';
import { MyWordbooksComponent } from './routes/main-screen/my-wordbooks/my-wordbooks.component';
import { DashboardComponent } from './routes/main-screen/dashboard/dashboard.component';
import { MainScreenComponent } from './routes/main-screen/main-screen.component';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

// import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NZ_I18N } from 'ng-zorro-antd/i18n';
import { en_US } from 'ng-zorro-antd/i18n';
import { registerLocaleData } from '@angular/common';
import en from '@angular/common/locales/en';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';
import { InforCourseComponent } from './routes/main-screen/personal-information/infor-course/infor-course/infor-course.component';
import { WordListDetailComponent } from './routes/learn/word-list/word-list-detail/word-list-detail/word-list-detail.component';
import { CreatedWorkbookComponent } from './routes/main-screen/created-workbook/created-workbook.component';
import { WordListComponent } from './routes/learn/word-list/word-list.component';
import { WordDetailComponent } from './routes/learn/word-detail/word-detail.component';
import { RegisterComponent } from './routes/Authorize/register/register.component';
import { InforDetailComponent } from './routes/main-screen/personal-information/infor-detail/infor-detail/infor-detail.component';
import { LoginComponent } from './routes/Authorize/login/login.component';
import { TokenInterceptor } from './interceptors/token.interceptor';
import { NgToastModule } from 'ng-angular-popup';
import { AdminComponent } from './routes/Authorize/admin/admin.component';
import { AdminCourseComponent } from './routes/Authorize/admin/admin-course/admin-course.component';
import { AdminPostComponent } from './routes/Authorize/admin/admin-post/admin-post.component';
import { AdminUserComponent } from './routes/Authorize/admin/admin-user/admin-user.component';
import { AdminWordComponent } from './routes/Authorize/admin/admin-word/admin-word.component';


registerLocaleData(en);

@NgModule({
  declarations: [
    AppComponent,
    MainScreenComponent,
    DashboardComponent,
    MyWordbooksComponent,
    AnalysisComponent,
    CommunityComponent,
    CreatedWorkbookComponent,
    PersonalInformationComponent,
    LearnComponent,
    WordListComponent,
    WordDetailComponent,
    InforDetailComponent,
    InforCourseComponent,
    WordListDetailComponent,
   LoginComponent,
    RegisterComponent,
    AdminComponent,
    AdminCourseComponent,
    AdminPostComponent,
    AdminUserComponent,
    AdminWordComponent

  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    NgZorroAntdModule,
    ReactiveFormsModule,
    NgToastModule
  ],
  providers: [{ provide: NZ_I18N, useValue: en_US },{provide: HTTP_INTERCEPTORS,useClass:TokenInterceptor,multi:true}],
  bootstrap: [AppComponent],
})
export class AppModule {}
