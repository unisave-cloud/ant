FROM mono:6.4.0

# copy the entire context into the /job folder
COPY ./ /job/

# go into the job folder
WORKDIR /job

# run the script, called "job.sh"
ENTRYPOINT ["bash"]
CMD ["job.sh"]
