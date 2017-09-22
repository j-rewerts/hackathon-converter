export const API_ENDPOINT = {
  ANALYZE_FILE: 'https://7d16342f-2af8-4e37-acd5-7bce44f2d6fa.mock.pstmn.io/analysis/start/abcd-efgh',
  GET_REPORTS: 'https://7d16342f-2af8-4e37-acd5-7bce44f2d6fa.mock.pstmn.io/analysis'
  // ANALYZE_FILE: 'https://jrewerts-project.appspot.com/analysis/start/',
  // GET_REPORTS: 'https://jrewerts-project.appspot.com/analysis'
};

export const GOOGLE_API = {
  CLIENT_ID: '902730555629-g2901alf3tbutjb2v6m91i6pevm28o4l.apps.googleusercontent.com',
  SCOPE: 'https://www.googleapis.com/auth/drive'
};

export const STORAGE_TOKEN_KEY = 'accessToken';

export const AppConfig = {
  toolbar: {
    toggleButton: {
      icon: {class: 'menu', type: 'md'}
    },
    brand: 'Hackathon',
    github: {
      icon: 'assets:github',
      hint: 'Github - Hackathon Converter',
      link: 'https://github.com/j-rewerts/hackathon-converter'
    }
  },
  sideMenu: {
    logo: 'assets:logo',
    title: 'Hackathon',
    appMenu: {
      name: 'appMenu',
      type: 'list',
      list: [
        {
          name: 'home',
          display: 'Home',
          type: 'link',
          icon: {class: 'home', type: 'md'},
          link: {path: '/home'}
        },
        {
          name: 'analyze',
          display: 'Analyze',
          type: 'link',
          icon: {class: 'playlist_add_check', type: 'md'},
          link: {path: '/reports'}
        },
        {
          name: 'convert',
          display: 'Convert',
          type: 'link',
          icon: {class: 'input', type: 'md'},
          link: {path: '/convert'}
        }
      ]
    },
  }
};
