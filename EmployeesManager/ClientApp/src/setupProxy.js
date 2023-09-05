const { createProxyMiddleware } = require('http-proxy-middleware');

let target = process.env.REACT_APP_PROXY_HOST;
if (target == null) {
    target = 'http://localhost:5000';
}

const context = [
    '/employees/hire',
    '/employees/delete',
    '/employees/employee',
    '/employees/fire',
    "/employees/getTree"
];

module.exports = function(app) {
  const appProxy = createProxyMiddleware(context, {
    proxyTimeout: 10000,
    target: target,
    secure: false,
    headers: {
      Connection: 'Keep-Alive'
    }
  });

  app.use(appProxy);
};
