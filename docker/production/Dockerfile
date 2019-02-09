FROM node:11.9.0-alpine
WORKDIR /kkpanel
COPY . /kkpanel
ENV NODE_ENV=production
ENV PORT=8080
RUN npm install --only=production
EXPOSE 8080
CMD ["npm", "start"]
