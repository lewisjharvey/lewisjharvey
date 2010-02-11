# Be sure to restart your server when you modify this file.

# Your secret key for verifying cookie session data integrity.
# If you change this key, all old sessions will become invalid!
# Make sure the secret is at least 30 characters and all random, 
# no regular words or you'll be exposed to dictionary attacks.
ActionController::Base.session = {
  :key         => '_mapmybeer_session',
  :secret      => '1961cc8738072850446551c81c539605ad1018a6f8a7fa0608e7f88033553a30a7cde3b6739aabdc85689801f6efc688f60519c47dfa76a53a0164e0ce6d62f0'
}

# Use the database for sessions instead of the cookie-based default,
# which shouldn't be used to store highly confidential information
# (create the session table with "rake db:sessions:create")
# ActionController::Base.session_store = :active_record_store
