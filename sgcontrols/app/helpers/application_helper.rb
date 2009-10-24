# Methods added to this helper will be available to all templates in the application.
module ApplicationHelper
  def generate_banner_class
    result = "bannerImage"
    if @current_controller == "site"
      case @current_action
      when "about"
        result += " about"
      when "commercial"
        result += " commercial"
      when "consultancy"
        result += " consultancy"
      when "contact"
        result += " contact"
      when "index"
          result += " home"
      when "installation"
          result += " installation"
      when "integration"
          result += " integration"
      when "knowledge"
          result += " knowledge"
      when "links"
          result += " links"                
      when "maintenance"
          result += " maintenance"
      when "partners"
          result += " partners"
      when "projects"
          result += " projects"
      when "residential"
          result += " residential"
      when "lonworks"
          result += " technology"
      when "dali"
          result += " technology"
      when "wireless"
          result += " technology"
      when "other"
          result += " technology"
      when "knx"
          result += " technology"
      else
          result += " home"
      end
      return result
    end
  end
end
