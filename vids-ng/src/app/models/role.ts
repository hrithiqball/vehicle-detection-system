export class Role {
  id: number = 0;
  roleName: string = '';
  userCount: number = 0;
}

export class RoleCC {
  roleName: string = '';
  userCount: number = 0;
  viewDashboardOperation: boolean = true;
  viewDashboardOverall: boolean = true;
  viewHighwayInfo: boolean = true;
  viewPublicInfo: boolean = true;
  viewUserFeedback: boolean = true;
  viewHighwayRating: boolean = true;
  viewReportAuditLog: boolean = true;
  viewReportHighwayInfoSummary: boolean = true;
  viewReportHighwayInfoComparison: boolean = true;
  viewReportPublicInfo: boolean = true;
  viewReportUserFeedback: boolean = true;
  viewAdminAreaRole: boolean = true;
  viewAdminAreaUser: boolean = true;
  viewAdminAreaHighway: boolean = true;
  viewAdminAreaAgency: boolean = true;

  editAdminAreaUser: boolean = true;
  editAdminAreaRole: boolean = true;
  editAdminAreaAgency: boolean = true;
  editAdminAreaHighway: boolean = true;
  editUserFeedback: boolean = true;
  editHighwayInfo: boolean = true;
  editPublicInfo: boolean = true;
  editHighwayRating: boolean = true;
}
