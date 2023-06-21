namespace Reefact.FluentRequestBinder {

    public interface RequestBinder {

        RequestConverter<TRequest> PropertiesOf<TRequest>(TRequest request);
        ArgumentsConverter         Arguments();

    }

}