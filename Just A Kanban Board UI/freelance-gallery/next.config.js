/** @type {import('next').NextConfig} */
module.exports = () => {
    const rewrites = () => {
      return [
        {
          source: "/kanbanboard/:path*",
          destination: "http://localhost:5154/kanbanboard/:path*",
        },
      ];
    };
    return {
      rewrites,
    };
  };