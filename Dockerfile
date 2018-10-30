FROM node:8.12
WORKDIR /kkpanel
ADD . /kkpanel
ENV NODE_ENV=production
ENV PORT=80
RUN npm install --production
EXPOSE 80
CMD ["npm", "start"]
