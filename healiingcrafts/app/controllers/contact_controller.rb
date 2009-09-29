class ContactController < ApplicationController
  def index
  end
  
  def send_message
    @from = params["from"]
    @body = params["body"]
    
    #send the message here
    sent = false
    if sent
      @message = "Success"
    else
      @message = "Fail"
    end
    render :action => "response"
    return
  end
end
